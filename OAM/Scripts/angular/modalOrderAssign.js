app.controller('OAMApp.Modals.OrderAssign', function ($scope, $modal, $log, $http) {
     
    $scope.AllUsers = [
       {
           id: 1,
           name: 'Administrator'
       },
       {
           id: 2,
           name: 'Hernando'
       }
    ];

    $scope.openAssign = function (size) {

        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: 'OAMApp.ModalInstance.OrderAssign',
            size: size,
            resolve: {
                users: function () {
                    return $scope.AllUsers;
                }
            }
        });

        modalInstance.result.then(function (selectedUser) {
            //$scope.selectedActiveUser = selectedUser;

            //$scope.OrdersList.AssignedTo = selectedUser;
            

            //make http post to update data.
            $http.post('/Orders/Assign', $scope.OrdersList).success(function (result) {
                $scope.OrdersList[0].AssignedTo = $scope.selectedActiveUser;
            });
            
          
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
});

    // Please note that $modalInstance represents a modal window (instance) dependency.
    // It is not the same as the $modal service used above.
app.controller('OAMApp.ModalInstance.OrderAssign', function ($scope, $modalInstance, users) {

    $scope.AllUsers = users;
    //$scope.selected = {
    //    item: $scope.items[0]
    //};


    $scope.modalText = "Assign orders.";

    $scope.ok = function () {
        $modalInstance.close($scope.selectedActiveUser);

        //$modalInstance.close();
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
});
