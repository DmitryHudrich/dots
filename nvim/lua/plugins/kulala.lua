return {
  "mistweaverco/kulala.nvim",
  keys = {
    {
      "<A-b>", function()
        require("kulala").jump_prev()
      end,
      desc = "jump prev",
    },
    {
      "<A-w>",
      function()
        require("kulala").jump_next()
      end,
      desc = "jump next",
    },
    {
      "<A-r>",
      function()
        require("kulala").run()
      end,
      desc = "send http",
    },
  },
  config = function()
    -- setup is required, even if you don't pass any options
    require("kulala").setup({
      -- default_view, body or headers
      default_view = "body",
      -- dev, test, prod, can be anything
      -- see: https://learn.microsoft.com/en-us/aspnet/core/test/http-files?view=aspnetcore-8.0#environment-files
      default_env = "dev",
      -- enable/disable debug mode
      debug = false,
      -- default formatters for different content types
      formatters = {
        json = { "jq", "." },
        xml = { "xmllint", "--format", "-" },
        html = { "xmllint", "--format", "--html", "-" },
      },
      -- default icons
      icons = {
        inlay = {
          loading = "⏳",
          done = "✅",
          error = "❌",
        },
        lualine = "🐼",
      },
      -- additional cURL options
      -- see: https://curl.se/docs/manpage.html
      additional_curl_options = {},
    })
  end,
}
