class Field {
    constructor(field, role) {
        this.field = field;
        this.role = role;

        var count = 0,
            userCount = 0,
            compCount = 20;
        var userHint = document.getElementById('user-hint'),
            compHint = document.getElementById('comp-hint');

        userHint.innerText = userCount;
        compHint.innerText = compCount;

        this.fire = (target) => {
            userCount = document.querySelectorAll('#field-comp .broken').length;
            if (target.classList.contains('sheep')) {
                target.classList.add('broken');
                userCount += 1;

            } else {
                target.classList.add('missed');
                count += 1;
            }
            if(userCount == 20) {
                alert('You WIN!!!!')
            }
            this.backFire();
            userHint.innerText = userCount;
            compHint.innerText = compCount;
        }

        this.backFire = () => {
            // функция устанавливает значение на поле юзера
            var targets = document.querySelectorAll('#field-user div');
            var sheeps = document.querySelectorAll('#field-user .sheep');
            if (count == 1 && sheeps.length > 0) {
                let firedItemIndex = Math.floor(Math.random() * targets.length);
                this.fire(targets[firedItemIndex]);
                compCount = sheeps.length - document.querySelectorAll('#field-user .broken').length;
                count = 0;
            }
            if (sheeps.length === 0) {
                alert('You LOST')
            }
        }
    }

    render() {
        var fieldBlock = document.getElementById('field-' + this.role)
        for (let i = 0; i < this.field.length; i++) {
            for (let j = 0; j < this.field[i].length; j++) {
                var block = document.createElement('div');
                if (this.field[i][j] === '+') {
                    block.classList.add('sheep');
                };
                if (this.role === 'comp') {
                    block.addEventListener('click', (event) => this.fire(event.target));
                };
                fieldBlock.appendChild(block)
            }
        }
    }
}

const userField = [
    ['.', '.', '.', '.', '.', '.', '.', '.', '.', '.'],
    ['.', '.', '.', '+', '+', '+', '.', '.', '+', '+'],
    ['.', '.', '.', '.', '.', '.', '.', '.', '.', '.'],
    ['.', '.', '.', '.', '.', '+', '.', '.', '.', '.'],
    ['.', '.', '.', '.', '.', '.', '.', '+', '+', '+'],
    ['.', '.', '.', '.', '+', '.', '.', '.', '.', '.'],
    ['+', '.', '.', '.', '+', '.', '+', '+', '.', '.'],
    ['.', '.', '.', '.', '+', '.', '.', '.', '.', '.'],
    ['.', '+', '+', '.', '+', '.', '+', '.', '+', '.'],
    ['.', '.', '.', '.', '.', '.', '.', '.', '.', '.']
]
const compField = [
    ['.', '.', '+', '.', '+', '.', '+', '.', '.', '.'],
    ['.', '.', '+', '.', '+', '.', '+', '.', '+', '.'],
    ['.', '.', '+', '.', '+', '.', '+', '.', '.', '.'],
    ['+', '.', '.', '.', '+', '.', '.', '.', '.', '.'],
    ['.', '.', '.', '.', '.', '.', '.', '.', '.', '.'],
    ['.', '.', '.', '.', '.', '.', '.', '.', '+', '.'],
    ['.', '.', '.', '.', '.', '.', '.', '.', '.', '.'],
    ['.', '+', '.', '.', '.', '.', '.', '.', '+', '.'],
    ['.', '+', '.', '.', '.', '.', '.', '.', '.', '.'],
    ['.', '.', '.', '+', '+', '.', '+', '+', '.', '.']
]

var gameu = new Field(userField, 'user')
gameu.render();

var gamec = new Field(compField, 'comp')
gamec.render();