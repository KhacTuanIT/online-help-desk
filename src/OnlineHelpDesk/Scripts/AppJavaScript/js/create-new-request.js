$(document).ready(function () {
    var factility = $("#facility");
    var equipment = $("#EquipmentId")
    factility.change(function () {
        var facilityId = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Home/GetEquipment",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {
                id: facilityId
            },
            success: function (data) {
                if (data != null || typeof (data) != 'undefined') {
                    equipment.html("");
                    equipment.append("<option>--- Choose Equipment ---</option>");
                    $.each(data, function (key, val) {
                        equipment.append('<option value="' + key + '">' + val + '</option>');
                    });
                }
                else {
                    equipment.html("");
                    equipment.append("<option>--- Not found equipments ---</option>");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                equipment.html("");
                equipment.append("<option>--- Not found equipments ---</option>");
            }
        });
    });
});