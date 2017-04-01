namespace TagItViewModels
{
    public class TagSearchModel
    {
        public string Query { get; set; }
        public PositionModel Center { get; set; }
        public BoundsModel Bounds { get; set; }
    }
}
