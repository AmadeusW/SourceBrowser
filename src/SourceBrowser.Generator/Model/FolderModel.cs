﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBrowser.Generator.Model
{
    public class FolderModel : IProjectItem
    {
        public ICollection<IProjectItem> Children { get; set; }

        public IProjectItem Parent { get; private set; }

        public string Name { get; set; }

        public string RelativePath { get; set; }

        public FolderModel(IProjectItem parent, string name, string path)
        {
            Parent = parent;
            Name = name;
            RelativePath = findRelativePath(path);
            Children = new List<IProjectItem>();    
        }

        private string findRelativePath(string path)
        {
            //Find the root WorkspaceModel
            IProjectItem currentNode = this;
            while (currentNode.Parent != null)
            {
                currentNode = currentNode.Parent;
            }

            string relativePath;
            string rootPath = ((WorkspaceModel)currentNode).ContainingPath;
            System.Diagnostics.Debug.WriteLine(rootPath);
            System.Diagnostics.Debug.WriteLine(path);
            relativePath = path.Remove(0, rootPath.Length);

            return relativePath;
        }

        public string GetPath()
        {
            return RelativePath;
        }
    }
}
