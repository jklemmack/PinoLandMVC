//
function PageLoad() {
    var tabs = location.hash.substr(1).split("&");
    for (var i = 0; i < tabs.length; i++) {
        //for the selected tab, find the parent widget

        //var tabPanel = $("#"+tabs[i]).parents(".ui-tabs").first();
        $("#" + tabs[i]).parents(".ui-tabs").first().tabs("option", "active", tabs[i]);
    }

}