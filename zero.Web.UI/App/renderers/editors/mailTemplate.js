
import Editor from 'zero/core/editor.ts';

const editor = new Editor('mailTemplate', '@mailTemplate.fields.');

const general = editor.tab('general', '@ui.tab_general');
const sender = editor.tab('sender', '@mailTemplate.tabs.sender');
const recipient = editor.tab('recipient', '@mailTemplate.tabs.recipient');

general.field('name', { label: '@ui.name' }).text(60).required();
general.field('key').text(60).required();
general.field('subject').text(80).required();
general.field('body').rte();
general.field('preheader').text(160);
sender.field('senderEmail').text();
sender.field('senderName').text();
recipient.field('recipientEmail').text();
recipient.field('cc').text();
recipient.field('bcc').text();

export default editor;