using System;
using System.Linq;

namespace LiteApi.Contracts.Models.ActionMatchingByParameters
{
    public class ActionMatchingWeight
    {
        private ActionMatchingWeight() { }

        public ActionContext ActionCtx { get; private set; }
        public int Weight { get; private set; }
        public PossibleParameterType[] Parameters { get; private set; }

        public static ActionMatchingWeight CalculateWeight(ActionContext actionCtx, PossibleParameterType[] parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (actionCtx == null) throw new ArgumentNullException(nameof(actionCtx));
            ActionMatchingWeight weight = new ActionMatchingWeight
            {
                ActionCtx = actionCtx,
                Parameters = parameters
            };
            weight.CalculateWeight();
            return weight;
        }

        private void CalculateWeight()
        {
            const int weightConst = 8500;
            int weight = 0;
            PossibleParameterType[] parametersMatchedByName = Parameters
                .Where(x => ActionCtx.Parameters.Any(y => y.Name == x.Name))
                .ToArray();

            // calculate for missing params
            foreach (var param in Parameters)
            {
                if (!ActionCtx.Parameters.Any(x => x.HasDefaultValue || parametersMatchedByName.Any(y => y.IsMatchedByName(x))))
                {
                    weight -= (int)(weightConst * 0.67);
                }
            }

            // calculate for extra params
            foreach (var param in ActionCtx.Parameters)
            {
                if (!parametersMatchedByName.Any(x => param.IsMatchedByName(x)))
                {
                    weight -= (int)(weightConst * 0.67);
                }
            }

            foreach (PossibleParameterType param in parametersMatchedByName)
            {
                ActionParameter actionParam = ActionCtx.Parameters.First(x => x.IsMatchedByName(param));
                int matchingWeight = weightConst - TypeWithPriority.GetTypePriority(actionParam.Type);
                if (!param.CanHandleType(actionParam.Type))
                {
                    matchingWeight = matchingWeight * -1;
                }
                weight += matchingWeight;
            }

            Weight = weight;
        }

        public override string ToString() => $"action: {ActionCtx}, weight: {Weight}";
    }
}
