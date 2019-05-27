function KeyPress(sender, args) {
    if (args.get_keyCharacter() == sender.get_numberFormat().DecimalSeparator) {
        args.set_cancel(true);
    }
}
function callAjaxRequest(sender, rags) {

    __doPostBack();
}