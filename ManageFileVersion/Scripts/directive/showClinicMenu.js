function showClinicMenu(app) {
var templateForm = '<input type="hidden" id={{name}}>';
templateForm += "<input class='form-control' id={{textname}}  ng-model='clinicName' type='text' ng-change='showClinicMenu(clinicName)'>";
    templateForm += "<div class='list-group'>";
    templateForm += "<a href='#' class='list-group-item'  ng-click='setClinicID(clinic.clinicIndex,clinic.clinicName)' ng-repeat='clinic in listClinics'>";
    templateForm += "{{clinic.clinicName}}  /  {{clinic.clinicAddr}}";
    templateForm += "</a>";
    templateForm += " </div>";

    app.directive("showclinicmenu", function () {
        return {
                restrict: 'E',                
                template: templateForm,
                controller: function ($scope, $attrs, $http) {
                    $scope.name = $attrs.name;
                    $scope.textname = $attrs.textname;
                    $scope.showClinicMenu = function myfunction(clinicName) {
                        $http.get('../api/ApiTest/listClinics/' + clinicName).then(function (value) {
                            $("#" + $attrs.name).empty();
                            $scope.listClinics = value.data;
                        });
                    }
                    $scope.setClinicID = function (clinicIndex, clinicName) {
                        $scope.listClinics = null; //清空資料
                        $scope.clinicName = clinicName;
                        $("#" + $attrs.name).val(clinicIndex);
                    }
                },
        }
});

    //var templateForm = "<input ng-model='" + clinicIndexName + "' type='text' id='" + clinicIndexName + "'>";
    //templateForm += "<input class='form-control' ng-model='clinicName' type='text' ng-change='showClinicMenu(clinicName)'>";
    //templateForm += "<div class='list-group'>";
    //templateForm += "<a href='#' class='list-group-item'  ng-click='setClinicID(clinic.clinicIndex,clinic.clinicName)' ng-repeat='clinic in listClinics'>";
    //templateForm += "{{clinic.clinicName}}";
    //templateForm += "</a>";
    //templateForm += " </div>";

    //app.directive("clinicindex", function () {
    //    return {
    //        restrict: "E",
    //        template: templateForm,
    //        transclude: true,
    //        controller: function ($scope, $http) {

    //            $scope.showClinicMenu = function myfunction(clinicName) {
    //                $http.get('../api/ApiTest/listClinics/' + clinicName).then(function (value) {                        
    //                    $("#" + clinicIndexName).empty();
    //                    $scope.listClinics = value.data;                        
    //                });
    //            }
    //            $scope.setClinicID = function (clinicIndex, clinicName) {
    //                $scope.listClinics = null; //清空資料
    //                $scope.clinicName = clinicName;
    //                $("#" + clinicIndexName).val(clinicIndex);
    //            }
    //        }
    //    };
    //});
}