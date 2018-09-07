﻿using LibGit2Sharp;
using System;
using System.IO;
using System.Linq;

namespace Lyra2.UtilShared
{
    public class GitStore
    {
        private readonly string repositoryRoot;

        private string GitIgnorePath => $"{this.repositoryRoot}\\.gitignore";

        public GitStore(string repositoryRoot)
        {
            this.repositoryRoot = repositoryRoot;
            if (!Directory.Exists($"{repositoryRoot}\\.git"))
            {
                Repository.Init(repositoryRoot);
            }
        }

        public void CommitFile(string file, string content)
        {
            using (var repo = new Repository(this.repositoryRoot))
            {
                var gitignore = $"# Lyra .gitignore to track single files{Environment.NewLine}*";
                if (File.Exists(this.GitIgnorePath))
                {
                    gitignore = File.ReadAllText(this.GitIgnorePath);
                }

                var fileName = Path.GetFileName(file);
                if (!gitignore.Contains($"!{fileName}"))
                {
                    gitignore += Environment.NewLine + $"!{fileName}";
                    File.WriteAllText(this.GitIgnorePath, gitignore);
                    if (repo.Index.All(s => s.Path != ".gitignore"))
                    {
                        repo.Index.Add(".gitignore");
                    }
                    Commands.Stage(repo, ".gitignore");
                }

                File.WriteAllText(file, content);
                if (repo.Index.All(s => s.Path != fileName))
                {
                    repo.Index.Add(fileName);
                }

                Commands.Stage(repo, fileName);
                var author = new Signature("Lyra", $"{Environment.UserName}@lyra", DateTime.Now);

                repo.Commit($"Lyra V{repo.Commits.Count() + 1}", author, author);
            }
        }
    }
}