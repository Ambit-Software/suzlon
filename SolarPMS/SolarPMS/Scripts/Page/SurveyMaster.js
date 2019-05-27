
function numberOnly(sender, eventArgs) {
    var k = eventArgs.get_keyCode()
    if (!((k >= 48 && k <= 57) || k == 8)) {
        debugger;
        eventArgs.set_cancel(true);
    }
}
function callAjaxRequest(sender, rags) {

    __doPostBack();
}


function KeyPress(sender, args) {
    if (args.get_keyCharacter() == sender.get_numberFormat().DecimalSeparator) {
        args.set_cancel(true);
    }
}
function NewKeyPress(sender, args) {
    var keyCharacter = args.get_keyCharacter();
    if (keyCharacter == sender.get_numberFormat().NegativeSign) {
        args.set_cancel(true);
    }
}