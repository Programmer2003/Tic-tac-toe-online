class Field {
    constructor(field, role) {
        this.field = field;
        this.role = role;
    }

    render() {
        var fieldBlock = document.getElementById('field-' + this.role)
        $(fieldBlock).empty();
        for (let i = 0; i < this.field.length; i++) {
            for (let j = 0; j < this.field[i].length; j++) {
                var block = document.createElement('div');
                let id = i * this.field.length + j;
                if (this.field[i][j] === '+' && this.role === 'user') {
                    block.classList.add('ship');
                };
                if (this.role === 'comp') {
                    block.dataset.id = id;
                    block.setAttribute("id", "sea-cell-" + id);
                }
                else {
                    block.setAttribute("id", "sea-my-cell-" + id);
                }
                fieldBlock.appendChild(block)
            }
        }
    }
}