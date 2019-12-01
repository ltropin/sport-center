(($) => {

    class Toggle {

        constructor(element, options) {

            this.defaults = {
                icon: 'fa-eye'
            };

            this.options = this.assignOptions(options);

            this.$element = element;
            this.$button = $(`<a class="btn-toggle-pass pr-3"><i class="fas ${this.options.icon}"></i></a>`);

            this.init();
        };

        assignOptions(options) {

            return $.extend({}, this.defaults, options);
        }

        init() {

            this._appendButton();
            this.bindEvents();
        }

        _appendButton() {
            this.$element.before(this.$button);
        }

        bindEvents() {

            this.$button.on('click touchstart', this.handleClick.bind(this));
        }

        handleClick() {

            let type = this.$element.attr('type');

            type = type === 'password' ? 'text' : 'password';

            this.$element.attr('type', type);
            this.$button.toggleClass('active');
        }
    }

    $.fn.togglePassword = function (options) {
        return this.each(function () {
            new Toggle($(this), options);
        });
    }

})(jQuery);
$(document).ready(function () {
    $('.mdb-select').materialSelect();
    $('#password').togglePassword();
    $('#password2').togglePassword();
})
