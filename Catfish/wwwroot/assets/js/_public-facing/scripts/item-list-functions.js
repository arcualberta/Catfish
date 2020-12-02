function filterItems(templateId, collectionId, fromDate, toDate, resultTableId) {

    $.ajax({

        url: '/api/items/GetItemList/',
        type: 'GET',
        data: {
            'templateId': templateId,
            "collectionId": collectionId,
            "startDate": fromDate,
            "endDate": toDate
        },

        success: function (data) {
            var resultsTableReference = "#" + resultTableId;

            //reset content table content rows
            $(resultsTableReference + " tr.tblRowContent").remove();

            $(data).each(function (index, d) {
                if (index > 0) {
                    var cols = d.split(",");
                    var row = ` <tr class="tblRowContent">`;
                    var i;
                    for (i = 0; i < cols.length - 1; i++) {
                        row += "<td>" + cols[i] + "</td>";
                    }
                    row += "</tr>";

                    $(resultsTableReference).append(row);
                    row = "";
                }
            });
        },
        error: function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });


}