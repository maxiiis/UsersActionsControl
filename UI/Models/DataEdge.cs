using GraphX.Common.Models;

namespace UI.Models
{
    public class DataEdge : EdgeBase<DataVertex>
    {
        public DataEdge(DataVertex source, DataVertex target, double weight = 1)
			: base(source, target, weight)
		{
		}
        public DataEdge()
            : base(null, null, 1)
        {
        }
        public string Text { get; set; }

        #region GET members
        public override string ToString()
        {
            return Text;
        }

        #endregion
    }
}
