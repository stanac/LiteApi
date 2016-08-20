namespace LiteApi.Contracts.Models.ActionMatchingByParameters
{
    public class PossibleParameterTypeSelection
    {
        public PossibleParameterType PossibleParam { get; set; }
        public TypeWithPriority MatchedOnType { get; set; }
        public int MatchingWeight
        {
            get
            {
                if (MatchedOnType == null) return 0;
                return MatchedOnType.TypePriority;
            }
        }
    }
}
