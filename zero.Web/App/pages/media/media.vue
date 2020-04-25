<template>
  <div class="media theme-dark">
    <ui-header-bar title="Media">
      <ui-search />
      <ui-button label="Add media" icon="fth-plus" />
    </ui-header-bar>

    <div class="ui-view-box">
      <div class="media-items">
        <a href="#" class="media-item" v-for="item in items">
          <img v-if="item.type === 'image'" :src="item.source" />
          <span class="media-item-content" v-if="item.type !== 'image'">
            <i :class="icons[item.type]" :data-extension="item.extension"></i>
            <span>{{item.source}}</span>
          </span>
        </a>
      </div>
    </div>
  </div>
</template>


<script>
  export default {

    data: () => ({
      items: [],
      icons: {
        image: 'fth-image',
        video: 'fth-video',
        file: 'fth-file'
      }
    }),

    created()
    {
      const sources = [
        'http://nolbert.com/wp-content/uploads/2018/04/nolbert_logitech_thumb_s.jpg',
        'http://nolbert.com/wp-content/uploads/2019/06/nolbert_orange_stack_01_thumb_sn.jpg',
        'http://nolbert.com/wp-content/uploads/2019/04/nolbert_vyvyd_thumb_violet_n.jpg',
        'http://nolbert.com/wp-content/uploads/2018/08/nolbert_oppo_r15_thumb_s.jpg',
        'http://nolbert.com/wp-content/uploads/2018/04/nolbert_asus_thumb_yellow_sq_n.jpg',
        'http://nolbert.com/wp-content/uploads/2017/06/nolbert_yas_darkbg_n.jpg',
        'http://nolbert.com/wp-content/uploads/2018/04/nolbert_atypical_thumb_s.jpg',
        'http://nolbert.com/wp-content/uploads/2018/01/nolbert_npci_book_shot_01_n.jpg',
        'http://nolbert.com/wp-content/uploads/2018/05/nolbert_fortune500_thumb_s.jpg',
        'http://nolbert.com/wp-content/uploads/2018/04/wired_thumb_s.jpg',
        'http://nolbert.com/wp-content/uploads/2018/04/nolbert_new_republic_thumb_s.jpg',
        'http://nolbert.com/wp-content/uploads/2018/05/nolbert_mm_smiley_thumb_n.jpg'
      ];

      sources.forEach(source =>
      {
        this.items.push({
          source: source,
          type: 'image'
        });
      });

      this.items.push({
        source: 'zeromagic-loop.webm',
        type: 'video',
        extension: '.webm'
      });

      this.items.push({
        source: 'documentation.docx',
        type: 'file',
        extension: '.docx'
      });
    },


    methods: {

    }
  }
</script>

<style lang="scss">
  .media
  {
    width: 100%;
    height: 100vh;
    background: var(--color-bg);
    overflow-y: auto;
  }

  .media-items
  {
    display: grid;
    grid-gap: var(--padding);
    grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
    align-items: stretch;
  }

  a.media-item
  {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    background: var(--color-bg-light);
    height: 210px;
    border-radius: var(--radius);
    overflow: hidden;
    color: var(--color-fg);
    font-size: var(--font-size-s);

    img
    {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }
  }

  .media-item-content
  {
    display: grid;
    width: 100%;
    grid-template-rows: 1fr auto;
    text-align: center;
    height: 100%;
    

    i
    {
      display: flex;
      width: 100%;
      align-items: center;
      justify-content: center;
      font-size: 28px;
      position: relative;

      &:after
      {
        font-family: var(--font);
        content: attr(data-extension);
        font-size: 12px;
        position: absolute;
        left: 50%;
        top: 50%;
        margin-top: 25px;
        background: var(--color-bg);
        display: inline-block;
        padding: 4px 8px;
        border-radius: 3px;
        font-weight: 600;
        transform: translateX(-50%);
      }
    }

    span
    {
      display: block;
      border-top: 1px solid var(--color-line);
      padding: 10px 20px 12px;
    }
  }
</style>