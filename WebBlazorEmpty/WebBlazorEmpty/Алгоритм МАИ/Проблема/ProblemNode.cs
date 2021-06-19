﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBlazorEmpty.AHP
{
    public interface IProblemNode : IStyled
    {
        INode Node { get; }
        IProblem Problem { get; }
        string Href { get; }
    }

    public class ProblemNode : IProblemNode, IStyled
    {
        public INode Node { get; set; }
        public IProblem Problem { get; set; }

        public string Href => $"node/{Node.Level}/{Node.Name}";

        public ProblemNode(IProblem problem, INode node)
        {
            Problem = problem;
            Node = node;
        }

        public string GetClass() => "";

        public string GetStyle() => "";
    }
}
