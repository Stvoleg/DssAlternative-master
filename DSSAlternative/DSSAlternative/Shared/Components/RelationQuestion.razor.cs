﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DSSAlternative.AHP;
using Microsoft.AspNetCore.Components;

namespace DSSAlternative.Shared.Components
{
    public partial class RelationQuestion : DSSComponentParamRelation
    {
        [Parameter]
        public EventCallback RelationUpdated { get; set; }
        [Parameter]
        public bool UseSafeWarnings { get; set; } = true;


        private IRating RatingNone { get; set; }
        private IRating RatingEqual { get; set; }
        private IEnumerable<(IRating forRating, IRating toRating)> Ratings { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Relation == null)
            {
                Relation = Problem.RelationsRequired.First();
            }

            CreateRatings();
            if (UseSafeWarnings && !Problem.GetMtxRelations(Relation.Main).WithZeros())
            {
                CreateMatrixes();
            }
        }
        private void CreateRatings()
        {
            RatingNone = new Rating(0);
            RatingEqual = new Rating(1);
            Ratings = RatingSystem.RatingsFor(Relation);
        }
        private void CreateMatrixes()
        {
            RatingMatrix.Add(RatingNone, GetMatrixForRating(0));
            RatingMatrix.Add(RatingEqual, GetMatrixForRating(1));
            foreach (var rating in Ratings)
            {
                double value = rating.forRating.Value;

                RatingMatrix.Add(rating.forRating, GetMatrixForRating(value));
                RatingMatrix.Add(rating.toRating, GetMatrixForRating(1 / value));
            }

            IMatrix GetMatrixForRating(double value)
            {
                IMatrixRelations source = Problem.GetMtxRelations(Relation.Main);
                source.Change(Relation.From, Relation.To, value);
                return source;
            }
        }

        private Dictionary<IRating, IMatrix> RatingMatrix { get; set; } = new Dictionary<IRating, IMatrix>();
        public string ClassSafeOrDangerous(IRating rating)
        {
            if (UseSafeWarnings && RatingMatrix.ContainsKey(rating))
            {
                return RatingMatrix[rating].Consistency.IsCorrect() ? "safe " : "dangerous ";
            }
            return "usual ";
        }
    }
}
