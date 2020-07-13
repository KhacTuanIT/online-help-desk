$(document).ready(function () {
    var _this = $("#notification");
    $.ajax({
        type: "GET",
        url: "/Notification/Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            _this.html("");
            if (data != null || typeof(data) != 'undefined') {
                $.each(data, function (key, val) {
                    console.log(val);
                    var text = '<a href="/To/' + key + '" class="dropdown-item">' +
                        '<div class="d-flex align-items-center">' +
                        '<div class="icon icon-sm bg-violet text-white"><i class="fa fa-envelope"></i></div>' +
                        '<div class="text ml-2">' +
                        '<p class="mb-0">' + val.Message + '</p>' +
                        '</div>' +
                        '</div>' +
                        '</a>';
                    _this.append(text);
                })
            }
            else {
                var text = '<a class="dropdown-item">' +
                    '<div class="d-flex align-items-center">' +
                    '<div class="icon icon-sm bg-gray-300 text-white"><i class="fa fa-info"></i></div>' +
                    '<div class="text ml-2">' +
                    '<p class="mb-0">Not found new notifications</p>' +
                    '</div>' +
                    '</div>' +
                    '</a>';
                _this.html(text);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR);
            _this.html("");
            var text = '<a class="dropdown-item">' +
                '<div class="d-flex align-items-center">' +
                '<div class="icon icon-sm bg-gray-300 text-white"><i class="fa fa-info"></i></div>' +
                '<div class="text ml-2">' +
                '<p class="mb-0">Not found new notifications</p>' +
                '</div>' +
                '</div>' +
                '</a>';
            _this.html(text);
        }
    });
});