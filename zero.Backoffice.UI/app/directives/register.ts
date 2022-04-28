
import { App } from 'vue';
import vClickOutside from './v-click-outside';
import vCurrency from './v-currency';
import vDate from './v-date';
import vFilesize from './v-filesize';
import vLocalize from './v-localize';
import vMaxLines from './v-max-lines';
import vPlaceholder from './v-placeholder';
import vResizeable from './v-resizeable';
import vSortable from './v-sortable';
import vEncode from './v-encode'
import vMultiline from './v-multiline';

const directives = [
  { key: 'click-outside', definition: vClickOutside },
  { key: 'currency', definition: vCurrency },
  { key: 'date', definition: vDate },
  { key: 'filesize', definition: vFilesize },
  { key: 'localize', definition: vLocalize },
  { key: 'max-lines', definition: vMaxLines },
  { key: 'placeholder', definition: vPlaceholder },
  { key: 'resizeable', definition: vResizeable },
  { key: 'sortable', definition: vSortable },
  { key: 'encode', definition: vEncode },
  { key: 'multiline', definition: vMultiline }
];

export default function (app: App)
{
  directives.forEach(directive =>
  {
    app.directive(directive.key, directive.definition);
  });
};