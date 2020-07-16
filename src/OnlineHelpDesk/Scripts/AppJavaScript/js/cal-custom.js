$(document).ready(function () {
    var created = $("#created-request");
    var assigned = $("#assigned-request");
    var processing = $("#processing-request");
    var completed = $("#completed-request");
    var closed = $("#closed-request");
    $.ajax({
        type: "GET",
        url: "/Home/GetCal",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data != null) {
                if (Object.keys(data).length != 0) {
                    $.each(data, function (key, val) {
                        created.html("");
                        created.html(val.Created);
                        assigned.html("");
                        assigned.html(val.Assigned);
                        processing.html("");
                        processing.html(val.Processing);
                        completed.html("");
                        completed.html(val.Completed);
                        closed.html("");
                        closed.html(val.Closed);
                    })
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });
});