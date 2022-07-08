/* 
 * Please note these are for ItemListBlock2, NOT ItemListBlock.
 * I don't know if these are moved over here properly or not bc that block doesn't seem to be active.
 * */

filterItems: function (entityTemplateId, collectionId) {

    //string templateId, string collectionId, string fromDate=null, string toDate=null
    var templateId = entityTemplateId;
    var collId = collectionId;
    var fromDate = $("#startDate").val();
    var toDate = $("#endDate").val();
    $.ajax({

        url: '/api/items/GetItemList/',
        type: 'GET',
        data: {
            'templateId': templateId, "collectionId": collId, "startDate": fromDate, "endDate": toDate
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


});