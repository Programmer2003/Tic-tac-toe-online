function showGames(data) {
    games.children().not(':first').remove();
    for (const raw of data) {
        games.append('<div class="col-xl-3 col-md-6 mb-4"><a href="#" data-id=' + raw.id + ' class="card border-0 shadow join-game" ><div class="card-body text-center"><h5 class="card-title mb-0 text-dark" >' + raw.author + '</h5><div class="card-text text-black-50" >' + raw.name + '</div></div></a></div>');
    }
}

function ShowTTTGameBoard() {
    $("#divGameFields").hide();
    $("#divGameBoard").show();
    for (var i = 0; i < 9; i++) {
        $("#divGameBoard").append("<span class='marker' data-id=" + i + " id='ttt-tile-" + i + "'></span>");
    }
}

function ShowBattleShipFields(field1, field2) {
    $("#divGameBoard").hide();
    $("#divGameFields").show();
    var gameu = new Field(field1, 'user')
    gameu.render();

    var gamec = new Field(field2, 'comp')
    gamec.render();
}