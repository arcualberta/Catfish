function filterItems(templateId, collectionId, fromDate, toDate, resultTableId, reportTemplateId) {
    let selectedReportTemplateId = reportTemplateId ? $(reportTemplateId).val() : null;
   
    $.ajax({

        url: '/api/items/GetItemList/',
        type: 'GET',
        data: {
            'templateId': templateId,
            "collectionId": collectionId,
            "startDate": fromDate,
            "endDate": toDate,
            "reportTemplate": selectedReportTemplateId
        },

        success: function (data) {
            var div = $("#itemListResultBlock");
            div.empty();
            div.html(data);
            //reset content table content rows

            ////////$("#itemListBlockTable tr.tblRowContent").remove();
            ////////$(data).each(function (index, d) {
            ////////    if (index > 0) {
            ////////        var cols = d.split(",");
            ////////        var row = `<tr class="tblRowContent">`;
            ////////        var i;
            ////////        for (i = 0; i < cols.length - 1; i++) {
            ////////            row += "<td>" + cols[i] + "</td>";
            ////////        }
            ////////        row += "</tr>";

            ////////        $("#itemListBlockTable").append(row);
            ////////        row = "";
            ////////    }
            ////////});
        },
        error: function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });


}