const aliases = Object.entries({
  '@zero/': '/',
  '@zerosetup/': '/setup/',
  '@shop/': '/../zero.Commerce/Plugins/zero.Commerce/'
});

export default {

  outDir: 'Assets/',

  base: '/zero/vue-cli',

  resolvers: [{
    alias(path)
    {
      for (const [alias, resolved] of aliases) 
      {
        if (path.startsWith(alias)) 
        {
          return path.replace(alias, resolved);
        }
      }
    }
  }],

  optimizeDeps: {
    include: ["qs"],
  },
};