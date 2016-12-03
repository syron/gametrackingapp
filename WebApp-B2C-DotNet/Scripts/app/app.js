
var app = angular.module('gameapp', ['angularMoment']);

app.factory('userService', function ($http) {
    return {
        users: function () {
            return $http.get('/api/pingis/users');
        },
        isregistered: function () {
            return $http.get('/api/pingis/isregistered');
        },
        register: function () {
            return $http.get('/api/pingis/register');
        },
        unregister: function () {
            return $http.get('/api/pingis/unregister');
        },
        highscore: function (by) {
            return $http.get('/api/pingis/users/toplist?by=' + by);
        }
    };
});

app.factory('matchService', function ($http) {
    return {
challenge: function (opponentId) {
            return $http.get('/api/matches/challenge?opponentId=' + opponentId);
        },
        matches: function (userId = null, status = null, top = null) {
            var data = {};
            if (userId !== null)
                data.userId = userId;
            if (status !== null)
                data.status = status;
            if (top !== null)
                data.top = top;
            return $http.get('/api/matches', {params: data });
        },
        matchesByUserId: function (userId) {
            return $http.get('/api/matches?userId=' + userId);
        },
        matchesByStatus: function (status) {
            return $http.get('/api/matches?status=' + status);
        },
        myChallengedMatches: function () {
            return $http.get('/api/matches/my/challenged');
        },
        myAcceptedMatches: function () {
            return $http.get('/api/matches/my/accepted');
        },
        myPlayedMatches: function () {
            return $http.get('/api/matches/my/played');
        },
        acceptMatch: function (matchId) {
            return $http.get('/api/matches/accept/' + matchId);
        },
        declineMatch: function (matchId) {
            return $http.get('/api/matches/decline/' + matchId);
        },
        finishMatch: function (matchId, challengerPoints, opponentPoints) {
            var data = {
                challengerPoints: challengerPoints,
                opponentPoints: opponentPoints
            };

            return $http.get('/api/matches/finish/' + matchId, { params: data });
        }
    };
});

app.constant("moment", moment);

app.controller('PingisCtrl', function (userService, matchService, moment, $scope) {
    $scope.users = [];
    $scope.matches = [];
    $scope.myinprogressmatches = [];
    $scope.myplayedmatches = [];
    $scope.myupcomingmatches = [];
    $scope.mychallengedmatches = [];
    $scope.currentUserIsRegistered = false;
    $scope.currentUserId = "";
    $scope.selectedUserToChallenge = null;
    $scope.selectedMatch = null;
    $scope.highscoreByMatchCount = [];
    $scope.highscoreByWinCount = [];

    $scope.currentDate = new moment();

    $scope.selectMatch = function (match) {
        $scope.selectedMatch = match;
    };

    $scope.saveMatch = function (match) {
        matchService.finishMatch(match.MatchId, match.ChallengerPoints, match.OpponentPoints).then(function (d) {
            toastr.success("Match Status", "Match finished.");
            $scope.loadMatches();
        });
    };

    $scope.checkIfRegistered = function () {
        userService.isregistered().then(function (d) {
            $scope.currentUserIsRegistered = d.data;
            if ($scope.currentUserIsRegistered)
                $scope.initPingisGame();
        });
    };

    $scope.registerMe = function () {
        userService.register().then(function (d) {
            if (d.data === true) {
                $scope.currentUserIsRegistered = true;
            } else {
                toastr.error("Already registered", "Game Registration");
            }
            $scope.initPingisGame();
        });
    };

    $scope.unregisterMe = function () {
        userService.unregister().then(function (d) {
            if (d.data === true) {
                $scope.currentUserIsRegistered = false;
            } else {
                toastr.error("Already registered", "Game Registration");
            }
        });
    };

    $scope.challenge = function () {
        matchService.challenge($scope.selectedUserToChallenge).then(function (d) {
            $scope.loadMatches();
        });
    };

    $scope.loadMatches = function () {
        // get all matches
        matchService.matches(null, 2).then(function (d) {
            var matches = d.data;
            $scope.matches = matches;
        });

        // get my played matches 
        matchService.matches($scope.currentUserId, 2, 5).then(function (d) {
            $scope.myplayedmatches = d.data;
        });

        // get accepted matches
        matchService.matches($scope.currentUserId, 1).then(function (d) {
            $scope.myupcomingmatches = d.data;
        });

        // get challenged matches
        matchService.matches($scope.currentUserId, 0).then(function (d) {
            $scope.mychallengedmatches = d.data;
        });
    };

    $scope.acceptMatch = function (matchId) {
        matchService.acceptMatch(matchId).then(function (d) {
            toastr.success("Match has been accepted", "Match status changed");
            $scope.loadMatches();
        });
    };
    $scope.declineMatch = function (matchId) {
        matchService.declineMatch(matchId).then(function (d) {
            toastr.error("Match has been declined", "Match status changed");
            $scope.loadMatches();
        });
    };

    $scope.loadUsers = function () {
        userService.users().then(function (d) {
            $scope.users = new Array();
            for (var i = 0; i < d.data.length; i++) {
                var user = d.data[i];

                if (user.UserId !== $scope.currentUserId) 
                    $scope.users.push(user);
            }
        });
    };

    $scope.loadHighScore = function () {
        userService.highscore('matchCount').then(function (d) {
            $scope.highscoreByMatchCount = d.data;
        });
        userService.highscore('winCount').then(function (d) {
            $scope.highscoreByWinCount = d.data;
        });
    }

    $scope.initPingisGame = function () {
        $scope.loadUsers();
        $scope.loadMatches();
        $scope.loadHighScore();
    };

    $scope.init = function (userId) {
        $scope.currentUserId = userId;
    };

    $scope.checkIfRegistered();
});
