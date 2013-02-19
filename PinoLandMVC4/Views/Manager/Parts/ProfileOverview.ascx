<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Fuqua.CompetativeAnalysis.MarketGame.Profile>" %>
<h3>
    Population</h3>
<div class="editor-label">
    <%: Html.LabelFor(model => model.TotalPopulation) %>
</div>
<div class="editor-field">
    <%: Html.EditorFor(model => model.TotalPopulation)%>
    <%: Html.ValidationMessageFor(model => model.TotalPopulation)%>
</div>
<h3>
    Probability Distribution</h3>
<table border="1">
    <tr>
        <td>
            &nbsp;
        </td>
        <%foreach (var wealth in Model.Economy.Wealths.OrderBy(w => w.DisplayOrder))
          {%>
        <td>
            <%: wealth.Name %>
        </td>
        <%} %>
    </tr>
    <%foreach (var age in Model.Economy.Ages.OrderBy(a => a.DisplayOrder))
      { %>
    <tr>
        <td>
            <%: age.Name %>
        </td>
        <%foreach (var wealth in Model.Economy.Wealths.OrderBy(w => w.DisplayOrder))
          { %><td class="edit" profile="<%=Model.Name%>" age="<%=age.Name%>" wealth="<%=wealth.Name %>"><%: Model.Profile_Age_Wealth.Single(paw=>paw.Age == age && paw.Wealth == wealth).Probability %></td>
        <%} %>
    </tr>
    <%} %>
</table>
<script language="javascript" type="text/javascript">
    $(function () {
        $('.edit').editable(function (value, settings) {
            $.post('/Manager/Update/<%:Model.EconomyId%>'
                , { type: 'Profile_Age_Wealth',
                    profile: $(this).attr('profile'),
                    age: $(this).attr('age'),
                    wealth: $(this).attr('wealth'),
                    value: value
                });
            return value;
        }, /* end of editable function, start of editable settings */{
        type: 'text',
        tooltip: 'Click to edit...',
        onblur: 'submit'
    });

});
</script>
