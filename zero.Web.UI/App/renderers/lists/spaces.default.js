
import List from 'zero/core/list.js';

const list = new List('spaces.default');

list.column('name').name();
list.column('createdDate').created();
list.column('isActive').active();

export default list;