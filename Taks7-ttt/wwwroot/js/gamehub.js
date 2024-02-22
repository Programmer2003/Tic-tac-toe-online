let habUrl = '/game';
var hubConnection = new signalR.HubConnectionBuilder().withUrl(habUrl).build();

async function start() {
    try {
        await hubConnection.start();
        console.log("signalr connected.");
        hubConnection.invoke('GetGames');
    } catch (err) {
        console.log(err);
        settimeout(start, 5000);
    }
};

hubConnection.onclose(async () => {
    await start();
});

start();

hubConnection.on('getGamesList', data => {
    showGames(data);
});

hubConnection.on('waitingForOpponent', data => {
    games.hide();
    choice.hide();
    game.hide();
    waiting.show();
    $('#title').html(data);
});

hubConnection.on('usersCount', data => {
    $('#user-count').html(data);
});

hubConnection.on('opponentFound', (type, data, title) => {
    games.hide();
    choice.hide();
    waiting.hide();
    game.show();
    $('#title').html(title);
    switch (type) {
        case 0:
            ShowTTTGameBoard();
            break;
        case 1:
            ShowBattleShipFields(data.field1, data.field2);
            break;
        default:
            alert("No such game");
            break;
    }
});

hubConnection.on('changeGameStatus', data => {
    $('#game-status').html(data);
});

hubConnection.on('info', data => {
    message.html(data);
    info.fadeIn("slow");
});

hubConnection.on('moveMade', data => {
    $("#" + data.position).addClass("notAvailable");
    $("#" + data.position).addClass(data.type);
});