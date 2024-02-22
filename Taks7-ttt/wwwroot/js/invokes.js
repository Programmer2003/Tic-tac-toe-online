$('.game-choice').on('click', function () {
    var name = getCookie('name');
    if (!name) name = 'no name';

    var type = $(this).data('group');
    hubConnection.invoke('AddGame', name, type);
})

$(document).on('click', '.marker, #field-comp div', function () {
    if ($(this).hasClass("notAvailable")) {
        return;
    }

    hubConnection.invoke("MakeAMove", $(this).data('id'));
});

$(document).on('click', '.join-game', function () {
    var id = $(this).data('id');
    var name = getCookie('name');
    if (!name) name = 'no name';

    hubConnection.invoke('JoinGame', id, name);
});