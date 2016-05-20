function showStatistics(app) {
    var templateForm = '<div>';
    templateForm +=        '<label>統計資料</label>';
    templateForm +=         '<table class="table">';
    templateForm +=            '<tr>';
    templateForm +=               '<th>產品名稱</th>';
    templateForm +=                '<th>bug等級</th>';
    templateForm +=                 '<th>完成筆數</th>';
    templateForm +=                 '<th>未完筆數</th>';
    templateForm +=                 '<th>暫援筆數</th>';
    templateForm +=            '</tr>';
    templateForm +=            '<tr ng-repeat="statics in listAllStatics ">';
    templateForm +=                 '<td>{{::statics.ProductName}}</td>';
    templateForm +=                 '<td>{{::statics.BugGrade  }}</td>';
    templateForm +=                 '<td>{{::statics.Complete}}</td>';
    templateForm +=                 '<td>{{::statics.NotComplete}}</td>';
    templateForm +=                 '<td>{{::statics.NotDeal}}</td>';
    templateForm +=            '</tr>';
    templateForm +=            '<tr ng-repeat-end>';
    templateForm +=                '<td>總計</td>';
    templateForm +=                '<td></td>';
    templateForm +=                '<td>{{totalComplete}}</td>';
    templateForm +=               '<td>{{totalNotComplete}}</td>';
    templateForm +=              '<td>{{totalNotDeal}}</td>';
    templateForm +=           '</tr>';
    templateForm +=      '</table>';
    templateForm +='</div>';
    app.directive('myCustomer', function () {
        return {
            template: templateForm
        };
    });
}