using System;
using System.Linq;

namespace LiteApi.Contracts.Models.ActionMatchingByParameters
{
    /// <summary>
    /// Class to work with action matching weight that is used in action overloading to find closest match to an action to HTTP request.
    /// </summary>
    public class ActionMatchingWeight
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="ActionMatchingWeight"/> class from being created.
        /// </summary>
        private ActionMatchingWeight() { }

        /// <summary>
        /// Gets the action context.
        /// </summary>
        /// <value>
        /// The action context.
        /// </value>
        public ActionContext ActionCtx { get; private set; }

        /// <summary>
        /// Gets the weight. Wight is used to tell which action is more of a match to a request.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public int Weight { get; private set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public PossibleParameterType[] Parameters { get; private set; }

        /// <summary>
        /// Calculates the weight.
        /// </summary>
        /// <param name="actionCtx">The action CTX.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Wight, how much the action is matched to the request</returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
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

        /// <summary>
        /// Calculates the weight. Wight is how much the action is matched to the request
        /// </summary>
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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"action: {ActionCtx}, weight: {Weight}";
    }
}
