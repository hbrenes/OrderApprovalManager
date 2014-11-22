app.controller('OAMApp.OrdersController', function ($scope, $http) {

    //cg-busy properties
    $scope.delay = 0;
    $scope.minDuration = 0;
    $scope.message = 'Please Wait...';
    $scope.backdrop = true;
    $scope.promise = null;
    $scope.templateUrl = '/Partials/custom-template.html';


    //order status
    $scope.OrderStatus = [
        {
            id: 1,
            name: 'Pending Approval'
        },
        {
            id: 2,
            name: 'Approved'
        },
        {
            id: 3,
            name: 'On Hold'
        },
        {
            id: 4,
            name: 'Order Entry Error'
        },
        {
            id: 5,
            name: 'Abandoned'
        }
    ];

    //temp message for loading users
    $scope.message = 'Loading Users...';

    $scope.promise = $http.get('/Home/GetAllUsers').success(function (data) {
        $scope.AllUsers = data;
        $scope.message = 'Please Wait...';
    });


    //$scope.AllUsers = [
    //   {
    //       id: 1,
    //       name: 'Administrator'
    //   },
    //   {
    //       id: 2,
    //       name: 'Hernando'
    //   }
    //];


    $scope.currentStatus = null;
    
    $scope.OrdersList = null;

    //Search
    $scope.searchOrderNo = '';
    $scope.searchFrom = '';
    $scope.searchTo = '';
    
    

    $scope.onlyUserOrders = false;

    $http.get('/Home/GetUserName').success(function (data) {
        $scope.userName = data;    
    });

    $scope.EmptyGrid = function () {
        $scope.OrdersList = null;
    }


    $scope.Search = function () {

        //check if order number is present
        if ($scope.searchOrderNo != null && $scope.searchOrderNo != '') {
            $scope.promise = $http.get('/Home/GetOrdersByOrderNo', {
                params: {
                    orderNo: $scope.searchOrderNo
                }
            }).success(function (data) {
                    //alert(data[0].OrderDate);
                for (var i = 0; i < data.Orders.length; i++) {
                    data.Orders[i].OrderDate = $scope.toDate(data.Orders[i].OrderDate);
                    }
                    $scope.OrdersList = data;
                });
        }
        else 
            if (($scope.searchOrderNo == null || $scope.searchOrderNo == '') && ($scope.searchFrom != null && $scope.searchFrom != '') && ($scope.searchTo != null && $scope.searchTo != '')) {
            $scope.promise = $http.get('/Home/GetOrdersByStatusAndDate', {
                params: {
                    statusId: $scope.currentStatus.id,
                    searchFrom: $scope.searchFrom,
                    searchTo: $scope.searchTo,
                    user: $scope.userName,
                    byUser: $scope.onlyUserOrders

                }
            }).success(function (data) {
                    //alert(data[0].OrderDate);
                for (var i = 0; i < data.Orders.length; i++) {
                    data.Orders[i].OrderDate = $scope.toDate(data.Orders[i].OrderDate);
                    }
                    $scope.OrdersList = data;
                });

            } else if (($scope.searchOrderNo == null || $scope.searchOrderNo == '')) {
                $scope.promise = $http.get('/Home/GetOrdersByStatus', {
                    params: {
                    statusId: $scope.currentStatus.id,
                    user: $scope.userName,
                    byUser: $scope.onlyUserOrders

                }
                }).success(function (data) {
                    //alert(data[0].OrderDate);
                        for (var i = 0; i < data.Orders.length; i++) {
                            data.Orders[i].OrderDate = $scope.toDate(data.Orders[i].OrderDate);
                }
                        $scope.OrdersList = data;
                });
            }

        
    }

    $scope.Clear = function () {
        $scope.searchFrom = '';
        $scope.searchTo = '';
        $scope.currentStatus = null;
        $scope.searchOrderNo = '';
        $scope.OrdersList = null;
    }

    $scope.toDate = function(stringDate) {
        var newDate = new Date(parseInt(stringDate.replace("/Date(", "").replace(")/", ""), 10));
        return newDate;
    };

    $scope.Assign = function () {

        for (var i = 0; i < $scope.OrdersList.Orders.length; i++) {
            if ($scope.OrdersList.Orders[i].Selected)
            {
                $scope.OrdersList.Orders[i].AssignedTo = $scope.OrdersList.userToAssign;
            }
        }

        //make http post to update data.
        $http.post('/Orders/Assign', $scope.OrdersList).success(function (result) {

            
            //$scope.OrdersList[0].AssignedTo = $scope.selectedActiveUser;
        });
    }

    

});