angular.module('Paginate', [])

    .filter('paginate', function (paginatorService) {
        return function (input, rowsPerPage) {
            if (!input) {
                return input;
            }

            if (rowsPerPage) {
                paginatorService.rowsPerPage = rowsPerPage;
            }

            paginatorService.itemCount = input.length;

            return input.slice(parseInt(paginatorService.page * paginatorService.rowsPerPage), parseInt((paginatorService.page + 1) * paginatorService.rowsPerPage + 1) - 1);
        };
    })

    //test
    .filter('forLoop', function () {
        return function (input, start, end) {
            input = new Array(end - start);
            for (var i = 0; start < end; start++, i++) {
                input[i] = start;
            }

            return input;
        };
    })

    .service('paginatorService', function ($rootScope) {
        this.page = 0;
        this.rowsPerPage = 10;
        this.itemCount = 0;
        this.limitPerPage = 10;

        this.setPage = function (page) {
            if (page > this.pageCount()) {
                return;
            }
            this.page = page;
        };

        this.nextPage = function () {
            if (this.isLastPage()) {
                return;
            }

            this.page++;
        };

        this.perviousPage = function () {
            if (this.isFirstPage()) {
                return;
            }

            this.page--;
        };

        this.firstPage = function () {
            this.page = 0;
        };

        this.lastPage = function () {
            this.page = this.pageCount() - 1;
        };

        this.isFirstPage = function () {
            return this.page == 0;
        };

        this.isLastPage = function () {
            return this.page == this.pageCount() - 1;
        };

        this.pageCount = function () {
            return Math.ceil(parseInt(this.itemCount) / parseInt(this.rowsPerPage));
        };

        this.lowerLimit = function () {
            var pageCountLimitPerPageDiff = this.pageCount() - this.limitPerPage;

            if (pageCountLimitPerPageDiff < 0) {
                return 0;
            }

            if (this.page > pageCountLimitPerPageDiff + 1) {
                return pageCountLimitPerPageDiff;
            }

            var low = this.page - (Math.ceil(this.limitPerPage / 2) - 1);

            return Math.max(low, 0);
        };
    })

    .directive('Paginator', function factory() {
        return {
            restrict: 'AE',
            controller: function ($scope, paginatorService) {
                $scope.Paginator = paginatorService;
            },
            templateUrl: '/Scripts/partials/pagination.html'
        };
    });
