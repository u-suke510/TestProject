function ChangeSelected()
{
    var checkbox = $(this).prev();
    if (checkbox.prop("checked"))
    {
        checkbox.prop("checked", false);
    }
    else
    {
        checkbox.prop("checked", true);
    }
}
function ClickBtnEdit(e)
{
    e.preventDefault();
    var button = $(this);

    var selected = $("input[type=checkbox]:checked");
    if (selected.length <= 0)
    {
        alert("Not Selected.");
        return;
    }

    var url = button.attr("href") + "/" + selected.attr("data-user-id");
    window.location.assign(url);
}
function ClickBtnDelete(e)
{
    e.preventDefault();
    var button = $(this);

    var selected = $("input[type=checkbox]:checked");
    if (selected.length <= 0)
    {
        alert("Not Selected.");
        return;
    }

    var ids = new Array();
    $.each(selected, function () {
        ids.push($(this).attr("data-user-id"));
    });

    $.ajax({
        url: button.attr("href"),
        type: "POST",
        data: { ids: ids },
        dataType: "json",
        success: function (result) {
            $("#lstContent > div > div:not(.header)").remove();
            $("#lstContent > div").append(result.content);
        }
    });
}
