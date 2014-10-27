using System;
using System.Drawing;
using System.Text;
using System.Web.UI.WebControls;

namespace QuickWeb.Controls
{
    public class DropDownCheckBoxList : CheckBoxList
    {
        //First row
        public string Title { get; set; }

        //Arrow Down
        public string ImageURL { get; set; }

        //JQuery base library
        public string JQueryURL { get; set; }

        //Expand or hide on start
        public bool OpenOnStart { get; set; }

        //alternative row color:
        public Color AltRowColor { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            if (string.IsNullOrEmpty(ImageURL))
                throw new Exception("ImageURL was not set.");

            //if (string.IsNullOrEmpty(JQueryURL))
            //    throw new Exception("JqueryURL was not set.");

            base.OnLoad(e);
        }

        public override void ClearSelection()
        {
            base.ClearSelection();
        }

        /// Display as a dropdown lis
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            //catch ForeColor:
            if (this.ForeColor.IsEmpty)
                this.ForeColor = Color.Black;

            //catch AltRowColor:
            if (this.AltRowColor.IsEmpty)
                this.AltRowColor = Color.GhostWhite;

            //catch border style:
            if (this.BorderStyle.Equals(BorderStyle.NotSet) ||
                 this.BorderStyle.Equals(BorderStyle.NotSet))
                this.BorderStyle = BorderStyle.Solid;

            //catch border color:
            if (this.BorderColor.IsEmpty)
                this.BorderColor = Color.Silver;

            //catch border width:
            if (this.BorderWidth.IsEmpty)
                this.BorderWidth = Unit.Pixel(1);

            //catch background width:
            if (this.BackColor.IsEmpty)
                this.BackColor = Color.White;

            StringBuilder sbCss = new StringBuilder();

            //css definition
            sbCss.Append("<style type=\"text/css\">");
            sbCss.Append(".{0}{{");
            if (this.Font.Italic)
                sbCss.Append("font-style:italic; ");
            if (this.Font.Bold)
                sbCss.Append("font-weight:bold; ");

            string textDecor = string.Empty;
            if (Font.Overline || Font.Underline || Font.Strikeout)
            {
                sbCss.Append("text-decoration:");
                if (this.Font.Overline)
                    sbCss.Append("overline ");
                if (this.Font.Strikeout)
                    sbCss.Append("line-through ");
                if (this.Font.Underline)
                    sbCss.Append("underline ");
                sbCss.Append("; ");
            }
            if (!ForeColor.IsEmpty)
                sbCss.Append("color:" + ForeColor.Name.Replace("ff", "#") + "; ");
            if (!Font.Size.IsEmpty)
                sbCss.Append("font-size:" + Font.Size + "; ");
            if (!BackColor.IsEmpty)
                sbCss.Append("background-color: " +
        BackColor.Name.Replace("ff", "#") + "; ");

            sbCss.Append("width: {1}; ");

            sbCss.Append(" border:" + BorderStyle + " " +
        BorderWidth + " " + this.BorderColor.Name.Replace("ff", "#") + "; }}.{0} ul  {{overflow-x:hidden; overflow-y:auto; height:{2}; margin-top:5px;  padding:0; border:solid 1px " +
        BorderColor.Name.Replace("ff", "#") + "; ");

            sbCss.Append("}} .{0} li {{list-style: none;}}</style>");

            string css = sbCss.ToString();

            //default css class
            if (string.IsNullOrEmpty(this.CssClass)) this.CssClass = "ddlchklst";

            //default width and height:
            if (Width.IsEmpty) Width = Unit.Pixel(170);
            if (Height.IsEmpty) Height = Unit.Pixel(170);

            //first row division:
            string divFirstRow = @"<div>   {0} <img id=""{1}""style=""float: right;"" src=""{2}"" /> </div>";

            //unorder list:
            string ulTag = "<ul style=\"display:{1};position: absolute; z-index:101;width:" + (Width.Value - 4) + "px\"; id=\"{0}\" >";

            //check box:
            string chkBox = "<input id=\"{0}\" name=\"{1}\" type=\"checkbox\" value=\"{2}\"{3} />";

            //attributes to render:
            string attrs = string.Empty;
            foreach (string key in this.Attributes.Keys)
            {
                attrs += " " + key + "=" + "\"" + this.Attributes[key].ToString() + "\"";
            }

            //title for check box:
            string label = "<label for=\"{0}\">{1}</label>";

            //toggle click
            string jqueryToggleFunction = @"<script type=""text/javascript"">$(document).ready(function () {{ $(""#{0}"").parent().click(function () {{ $(""#{1}"").toggle(""fast"");  }});   $(""#{0}"").parent().parent().find(':checkbox').eq(0).change(function (e) {{ if (e.target.checked) {{ $(e.target).closest('ul').find(':checkbox').attr('checked', true); }} else {{ $(e.target).closest('ul').find(':checkbox').attr('checked', false); }}  }});   $("".{2} li"").css(""width"", """ + (Width.Value - 20) +
                "\"); $(\".{2} li:odd\").css(\"background-color\", \"#FFFFFF\"); $(\".{2} li:even\").css(\"background-color\", \"#F0F4FB\"" +
                /* AltRowColor.Name.Replace("ff", "#") + */ "); }});  </script>";

            //*************  rendering  ***********************//

            //render css:
            writer.WriteLine(string.Format(css, CssClass, Width, Height));

            // render jquery url:
            // writer.WriteLine(string.Format("<script type='text/javascript' src='{0}'></script>", JQueryURL));

            //render toggle click function:
            writer.Write(string.Format(jqueryToggleFunction, base.ClientID +
        "_arrowDown", base.ClientID + "_ul", this.CssClass));

            //render the div start tag:
            writer.WriteLine(string.Format("<div class=\"{0}\">", this.CssClass));

            //render first row with the title:
            writer.Write(string.Format(divFirstRow, this.Title + "  ",
        base.ClientID + "_arrowDown", ImageURL));
            writer.WriteLine();
            writer.Indent++;

            //render ul start tag:
            writer.WriteLine(string.Format(ulTag, base.ClientID + "_ul",
            OpenOnStart ? "block" : "none"));

            // render a checkbox for selecting all, that will toggle all the selection
            # region checkboxForAll

            writer.Indent++;
            writer.WriteLine("<li>");
            writer.Indent++;
            writer.WriteLine(string.Format(chkBox,
                base.ClientID + "_0",
                base.ClientID + "$0",
                "All", " "
                ));
            writer.WriteLine(string.Format(label, base.ClientID + "_0"
    , "All"));
            writer.Indent--;

            writer.WriteLine("</li>");
            writer.WriteLine();
            writer.Indent--;

            # endregion

            //render the check box list itself:
            for (int index = 0; index < Items.Count; index++)
            {
                writer.Indent++;
                writer.WriteLine("<li>");
                writer.Indent++;
                writer.WriteLine(string.Format(chkBox,
                    base.ClientID + "_" + (index + 1).ToString(),
                    base.ClientID + "$" + (index + 1).ToString(),
                    Items[index].Value,
                    (Items[index].Selected ? " checked=true" : " ")
                    ));
                writer.WriteLine(string.Format(label, base.ClientID + "_" +
        (index + 1).ToString(), Items[index].Text + " "));
                writer.Indent--;

                writer.WriteLine("</li>");
                writer.WriteLine();
                writer.Indent--;
            }

            //render end ul tag:
            writer.WriteLine("</ul>");

            //render end div tag:
            writer.WriteLine("</div>");
        }
    }
}