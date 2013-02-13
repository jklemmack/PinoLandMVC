using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Security;

using MG = Fuqua.CompetativeAnalysis.MarketGame;

namespace System.Web.Mvc.Html
{
    public static class Html
    {
        public static string RoleActionLink(this HtmlHelper<object> html, string role, string linkText, IDictionary<string, object> htmlAttributes, string actionName, string controllerName)
        {
            if (html.ViewContext.HttpContext.User.IsInRole(role))
            {
                var x = html.ActionLink(linkText, actionName, controllerName, null, htmlAttributes);
                return x.ToHtmlString();
            }
            else
                return String.Empty;
        }

        public static string Table(this HtmlHelper helper, string name, IList items)
        {
            if (items == null || items.Count == 0 || string.IsNullOrEmpty(name))
                return string.Empty;

            return BuildTable(name, items, new Dictionary<string, object>());
        }

        public static string Table(this HtmlHelper helper, string name, IList items, IDictionary<string, object> attributes)
        {
            if (items == null || items.Count == 0 || string.IsNullOrEmpty(name))
                return string.Empty;

            return BuildTable(name, items, attributes);
        }

        private static string BuildTable(string name, IList items, IDictionary<string, object> attributes)
        {
            StringBuilder sb = new StringBuilder();
            BuildTableHeader(sb, items[0].GetType());

            foreach (var item in items)
            {
                BuildTableRow(sb, item);
            }

            TagBuilder builder = new TagBuilder("table");
            builder.MergeAttributes(attributes);
            builder.MergeAttribute("name", name);
            builder.InnerHtml = sb.ToString();
            return builder.ToString(TagRenderMode.Normal);
        }

        private static void BuildTableRow(StringBuilder sb, object obj)
        {
            Type objType = obj.GetType();
            sb.AppendLine("\t<tr>");
            foreach (var property in objType.GetProperties())
            {
                sb.AppendFormat("\t\t<td>{0}</td>\n", property.GetValue(obj, null));
            }
            sb.AppendLine("\t</tr>");
        }

        private static void BuildTableHeader(StringBuilder sb, Type p)
        {
            sb.AppendLine("\t<tr>");
            foreach (var property in p.GetProperties())
            {
                sb.AppendFormat("\t\t<th>{0}</th>\n", property.Name);
            }
            sb.AppendLine("\t</tr>");
        }
    }
}