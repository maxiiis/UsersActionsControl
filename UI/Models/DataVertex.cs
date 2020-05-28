using GraphX.Common.Models;

namespace UI.Models
{
    public class DataVertex: VertexBase
    {
        public string Text { get; set; }
 
        #region Calculated or static props

        public override string ToString()
        {
            return Text;
        }

        #endregion

        public DataVertex():this(string.Empty)
        {
        }

        public DataVertex(string text = "")
        {
            Text = text;
        }
    }
}
