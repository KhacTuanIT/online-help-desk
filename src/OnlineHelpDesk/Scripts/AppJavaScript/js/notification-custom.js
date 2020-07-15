$(document).ready(function () {
    var _this = $("#notification");
    $.ajax({
        type: "GET",
        url: "/Notification/Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data != null) {
                if (Object.keys(data).length != 0) {
                    _this.html("");
                    $.each(data, function (key, val) {
                        var text = '<a href="/To/' + key + '" class="dropdown-item">' +
                            '<div class="d-flex align-items-center">' +
                            '<div class="icon icon-sm bg-violet text-white"><i class="fa fa-envelope"></i></div>' +
                            '<div class="text ml-2">' +
                            '<p class="mb-0">' + val.Message + '</p>' +
                            '</div>' +
                            '</div>' +
                            '</a>';
                        _this.append(text);
                    });
                }
                else {
                    _this.html("");
                    var text = '<a class="dropdown-item">' +
                        '<div class="d-flex align-items-center">' +
                        '<div class="icon icon-sm bg-gray-300 text-white"><i class="fa fa-info"></i></div>' +
                        '<div class="text ml-2">' +
                        '<p class="mb-0">Not found new notifications</p>' +
                        '</div>' +
                        '</div>' +
                        '</a>';
                    _this.append(text);
                }
            }
            else {
                _this.html("");
                var text = '<a class="dropdown-item">' +
                    '<div class="d-flex align-items-center">' +
                    '<div class="icon icon-sm bg-gray-300 text-white"><i class="fa fa-info"></i></div>' +
                    '<div class="text ml-2">' +
                    '<p class="mb-0">Not found new notifications</p>' +
                    '</div>' +
                    '</div>' +
                    '</a>';
                _this.append(text);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _this.html("");
            var text = '<a class="dropdown-item">' +
                '<div class="d-flex align-items-center">' +
                '<div class="icon icon-sm bg-gray-300 text-white"><i class="fa fa-info"></i></div>' +
                '<div class="text ml-2">' +
                '<p class="mb-0">Not found new notifications</p>' +
                '</div>' +
                '</div>' +
                '</a>';
            _this.append(text);
        }
    });

    $.ajax({
        type: "GET",
        url: "/Notification/GetRole",
        success: function (data) {
            if (data != null) {
                $("#userRole").html(data);
            }
            else {
                $("#userRole").html("Guest");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#userRole").html("Guest");
        }
    });
});