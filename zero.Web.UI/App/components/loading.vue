<template>
  <i class="ui-loading"></i>
</template>


<script>
  export default {
    name: 'uiLoading'
  }
</script>

<style lang="scss">
.ui-loading
{   
  display: inline-block;
  width: 28px;
  height: 28px;
  border-radius: 40px;
  border: 2px solid var(--color-bg-mid);
  border-left-color: var(--color-fg);
  opacity: 1;
  will-change: transform;
  animation: loadingRotation .8s linear infinite;
  transition: opacity .25s ease;
}

@keyframes loadingRotation
{
  0%
  {
    transform: rotate(0)
  }
  100%
  {
    transform: rotate(1turn)
  }
}
</style>