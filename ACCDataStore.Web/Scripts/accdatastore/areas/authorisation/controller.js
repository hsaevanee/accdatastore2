angular.module('root.controllers', ['ngSanitize', 'ui.select', 'highcharts-ng', 'datatables'])

.controller('rootCtrl', function ($scope, $rootScope) {
})

.controller('indexCtrl', function ($scope, $rootScope, $state, $stateParams, $timeout, indexService) {

    $scope.mIndex = {};

    //$('#login-form-link').click(function (e) {
    //    $("#login-form").delay(100).fadeIn(100);
    //    $("#register-form").fadeOut(100);
    //    $('#register-form-link').removeClass('active');
    //    $(this).addClass('active');
    //    e.preventDefault();
    //});
    //$('#register-form-link').click(function (e) {
    //    $("#register-form").delay(100).fadeIn(100);
    //    $("#login-form").fadeOut(100);
    //    $('#login-form-link').removeClass('active');
    //    $(this).addClass('active');
    //    e.preventDefault();
    //});

    var check = function () {
        if (document.getElementById('password').value ==
            document.getElementById('confirmpassword').value) {
            document.getElementById('message').style.color = 'green';
            document.getElementById('message').innerHTML = 'matching';
        } else {
            document.getElementById('message').style.color = 'red';
            document.getElementById('message').innerHTML = 'not matching';
        }
    }

    $scope.doLogin = function (eUser) {

            $scope.bShowError = false;



            indexService.login(eUser).then(function (response) {

                $rootScope.bShowLoading = false;

                if (response.data.IsRedirect) {

                    window.location.href = response.data.RedirectUrl;

                } else {

                    $scope.bShowError = true;

                    $scope.errorType = response.data.ErrorType;

                    $scope.errorMessage = response.data.ErrorMessage;

                }

            }, function (response) {

                $rootScope.bShowLoading = false;

            });

        };

    //6. create resetForm() function. This will be called on Reset button click.  
    $scope.resetForm = function () {
        window.location.href = '/'; //back to root page
    };

    //6. create resetForm() function. This will be called on Reset button click.  
    $scope.SendEmail = function () {
        window.location.href = '/'; //back to root page
    };

})



.controller('listCtrl', function ($scope, $rootScope, $state, $stateParams, userSettingsService) {
    $rootScope.pageTitle = "List User";
    $scope.mList = {};

    userSettingsService.getAll().then(function (response) {
        $scope.mList = response.data;
    }, function (response) {
    });

    $scope.doAdd = function () {
        $state.go('add');
    }

    $scope.doView = function (eUser) {
        $state.go('view', { nID: eUser.ID });
    }

    $scope.doEdit = function (eUser) {
        $state.go('edit', { nID: eUser.ID });
    }

    $scope.doDelete = function (eUser) {
        if (confirm("Confirm delete ?")) {
            userSettingsService.delete(eUser.ID).then(function (response) {
                $scope.mList = response.data;
            }, function (response) {
            });
        }
    }
})

.controller('addCtrl', function ($scope, $rootScope, $state, $stateParams, userSettingsService) {
    $rootScope.pageTitle = "Add User";
    $scope.mAddEditView = {};

    $scope.isShowSaveButton = true;

    userSettingsService.getDefault().then(function (response) {
        $scope.mAddEditView = response.data;
    }, function (response) {
    });

    $scope.doSave = function (eUser) {
        userSettingsService.save(eUser).then(function (response) {
            $state.go('list');
        }, function (response) {
        });
    }

    $scope.doCancel = function () {
        $state.go('list');
    }
})

.controller('editCtrl', function ($scope, $rootScope, $state, $stateParams, userSettingsService) {
    $rootScope.pageTitle = "Edit User";
    $scope.mAddEditView = {};

    $scope.isShowSaveButton = true;

    userSettingsService.getByID($stateParams.nID).then(function (response) {
        $scope.mAddEditView = response.data;
    }, function (response) {
    });

    $scope.doSave = function (eUser) {
        userSettingsService.save(eUser).then(function (response) {
            $state.go('list');
        }, function (response) {
        });
    }

    $scope.doCancel = function () {
        $state.go('list');
    }
})

.controller('viewCtrl', function ($scope, $rootScope, $state, $stateParams, userSettingsService) {
    $rootScope.pageTitle = "View User";
    $scope.mAddEditView = {};

    $scope.isShowSaveButton = false;

    userSettingsService.getByID($stateParams.nID).then(function (response) {
        $scope.mAddEditView = response.data;
    }, function (response) {
    });

    $scope.doCancel = function () {
        $state.go('list');
    }
});

