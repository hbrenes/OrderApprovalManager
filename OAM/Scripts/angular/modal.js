app.controller('OAMApp.Modals', function ($scope, $modal, $log, $http) {

    $scope.items = ['item1', 'item2', 'item3'];

    $scope.open = function (size) {

        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: 'OAMApp.ModalInstance',
            size: size,
            resolve: {
                items: function () {
                    return $scope.items;
                }
            }
        });

        modalInstance.result.then(function (selectedItem) {
            //$scope.selected = selectedItem;

            //make http post to update data.
            $scope.promise = $http.post('/Orders/Update', $scope.Order).success(function (result) {
                alert("Record Updated!");
            });
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
});

    // Please note that $modalInstance represents a modal window (instance) dependency.
    // It is not the same as the $modal service used above.
app.controller('OAMApp.ModalInstance', function ($scope, $modalInstance, items) {

    $scope.items = items;
    //$scope.selected = {
    //    item: $scope.items[0]
    //};
    $scope.modalText = "Do you want to update this order?";
    $scope.ok = function () {
        //$modalInstance.close($scope.selected.item);

        $modalInstance.close();
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
});
