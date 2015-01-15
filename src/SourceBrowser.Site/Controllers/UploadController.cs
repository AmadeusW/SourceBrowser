namespace SourceBrowser.Site.Controllers
{
    using System.IO;
    using System.Web.Mvc;
    using SourceBrowser.SolutionRetriever;
    using SourceBrowser.Generator.Transformers;
    using SourceBrowser.Site.Repositories;
    using System;
    using SourceBrowser.Utils;

    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Submit(string githubUrl)
        {
            RequestUtils.CreateRequestId();
            // If someone navigates to submit directly, just send 'em back to index
            if (string.IsNullOrWhiteSpace(githubUrl))
            {
                return View("Index");
            }

            var retriever = new GitHubRetriever(githubUrl);
            if (!retriever.IsValidUrl())
            {
                ViewBag.Error = "Make sure that the provided path points to a valid GitHub repository.";
                return View("Index");
            }

            LoggingUtils.Info("Begin downloading " + githubUrl);
            string repoRootPath = string.Empty;
            try
            {
                repoRootPath = retriever.RetrieveProject();
            }
            catch (Exception ex)
            {
                LoggingUtils.Error("Error downloading repository: " + ex.ToString());
                ViewBag.Error = "There was an error downloading this repository.";
                return View("Index");
            }

            // Generate the source browser files for this solution
            var solutionPaths = GetSolutionPaths(repoRootPath);
            if (solutionPaths.Length == 0)
            {
                LoggingUtils.Warning("No solution was found in " + repoRootPath);
                ViewBag.Error = "No C# solution was found. Ensure that a valid .sln file exists within your repository.";
                return View("Index");
            }

            var organizationPath = System.Web.Hosting.HostingEnvironment.MapPath("~/") + "SB_Files\\" + retriever.UserName;
            var repoPath = Path.Combine(organizationPath, retriever.RepoName);

            // TODO: Use parallel for.
            foreach (var solutionPath in solutionPaths)
            {
                LoggingUtils.Info("Begin processing solution " + solutionPath);
                Generator.Model.WorkspaceModel workspaceModel;
                try
                {
                    workspaceModel = UploadRepository.ProcessSolution(solutionPath, repoRootPath);
                }
                catch (Exception ex)
                {
                    LoggingUtils.Error("Error processing solution", ex);
                    ViewBag.Error = "There was an error processing solution " + Path.GetFileName(solutionPath);
                    return View("Index");
                }

                //One pass to lookup all declarations
                LoggingUtils.Info("Invoking TokenLookupTransformer");
                var typeTransformer = new TokenLookupTransformer();
                typeTransformer.Visit(workspaceModel);
                var tokenLookup = typeTransformer.TokenLookup;

                //Another pass to generate HTMLs
                LoggingUtils.Info("Invoking HtmlTransformer");
                var htmlTransformer = new HtmlTransformer(tokenLookup, repoPath);
                htmlTransformer.Visit(workspaceModel);

                LoggingUtils.Info("Invoking SearchIndexTransformer");
                var searchTransformer = new SearchIndexTransformer(retriever.UserName, retriever.RepoName);
                searchTransformer.Visit(workspaceModel);

                // Generate HTML of the tree view
                LoggingUtils.Info("Invoking TreeViewTransformer");
                var treeViewTransformer = new TreeViewTransformer(repoPath, retriever.UserName, retriever.RepoName);
                treeViewTransformer.Visit(workspaceModel);

                LoggingUtils.Info("Finished processing solution " + solutionPath);
            }

            return Redirect("/Browse/" + retriever.UserName + "/" + retriever.RepoName);
        }

        /// <summary>
        /// Simply searches for the solution files and returns their paths.
        /// </summary>
        /// <param name="rootDirectory">
        /// The root Directory.
        /// </param>
        /// <returns>
        /// The solution paths.
        /// </returns>
        private string[] GetSolutionPaths(string rootDirectory)
        {
            return Directory.GetFiles(rootDirectory, "*.sln", SearchOption.AllDirectories);
        }
    }
}