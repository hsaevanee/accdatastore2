angular.module('root.services', [])

.factory('indexService', function ($http, $rootScope) {
    return {
        getCondition: function () {
            return $http.get(sContextPath + "SchoolProfiles/PrimarySchoolProfile/GetCondition");
        },
        getData: function (listSeedCode, sYear) {
            $rootScope.bShowLoading = true;
            return $http.get(sContextPath + "SchoolProfiles/PrimarySchoolProfile/GetData", { params: { "listSeedCode": listSeedCode, "sYear": sYear } });
        },
        exportData: function (listSeedCode, sYear, tablename) {
            $rootScope.bShowLoading = true;
            return $http.get(sContextPath + "SchoolProfiles/PrimarySchoolProfile/ExportData/", { params: { "listSeedCode": listSeedCode, "sYear": sYear, "tablename": tablename } });
        },
        getReport: function (listSeedCode, sYear) {
            $rootScope.bShowLoading = true;
            return $http.get(sContextPath + "SchoolProfiles/PrimarySchoolProfile/GetReport", { params: { "listSeedCode": listSeedCode, "sYear": sYear} });
        },
    };
})

.factory('Excel', function ($window) {
    var uri = 'data:application/vnd.ms-excel;base64,',
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
        base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
        format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
    return {
        tableToExcel: function (tableId, worksheetName) {
            var table = $(tableId),
                ctx = { worksheet: worksheetName, table: table.html() },
                href = uri + base64(format(template, ctx));
            return href;
        }
    };
});
