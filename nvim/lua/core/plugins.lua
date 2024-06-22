local lazypath = vim.fn.stdpath("data") .. "/lazy/lazy.nvim"

if not (vim.uv or vim.loop).fs_stat(lazypath) then
	vim.fn.system({
		"git",
		"clone",
		"--filter=blob:none",
		"https://github.com/folke/lazy.nvim.git",
		"--branch=stable", -- latest stable release
		lazypath,
	})
end
vim.opt.rtp:prepend(lazypath)

require("lazy").setup({
	{ "phaazon/hop.nvim" },
	{
		"nvim-neo-tree/neo-tree.nvim",
		branch = "v3.x",
		dependencies = {
			"nvim-lua/plenary.nvim",
			"nvim-tree/nvim-web-devicons", -- not strictly required, but recommended
			"MunifTanjim/nui.nvim",
			-- "3rd/image.nvim", -- Optional image support in preview window: See `# Preview Mode` for more information
		},
	},
	{ "nvim-treesitter/nvim-treesitter" },
	{ "neovim/nvim-lspconfig" },
	{
		"catppuccin/nvim",
		name = "catppuccin",
		priority = 1000,
		opts = {
			transparent_background = true,
		},
	},
	{ "neovim/nvim-lspconfig" },
	{ "hrsh7th/cmp-nvim-lsp" },
	{ "hrsh7th/cmp-buffer" },
	{ "hrsh7th/cmp-path" },
	{ "hrsh7th/cmp-cmdline" },
	{ "hrsh7th/nvim-cmp" },
	{ "williamboman/mason.nvim" },
	{ "williamboman/mason-lspconfig.nvim" },
	{ "mhartington/formatter.nvim", enabled = false },
	{ "nvim-telescope/telescope.nvim", tag = "0.1.6", dependencies = { "nvim-lua/plenary.nvim" } },
	{ "junegunn/fzf" },
	{ "junegunn/fzf.vim" },
	{ "prabirshrestha/asyncomplete.vim" },
	{
		"folke/which-key.nvim",
		lazy = false,
		init = function()
			vim.o.timeout = true
			vim.o.timeoutlen = 300
		end,
	},
	{ "hrsh7th/vim-vsnip" },
	{ "hrsh7th/vim-vsnip-integ" },
	{ "pocco81/auto-save.nvim" },
	{
		"stevearc/dressing.nvim",
		lazy = false,
		opts = {},
	},
	-- lazy.nvim
	{
		"folke/noice.nvim",
		event = "VeryLazy",
		opts = {
			background_colour = "#000000",
		},
		dependencies = {
			-- if you lazy-load any plugin below, make sure to add proper `module="..."` entries
			"MunifTanjim/nui.nvim",
			-- OPTIONAL:
			--   `nvim-notify` is only needed, if you want to use the notification view.
			--   If not available, we use `mini` as the fallback
			"rcarriga/nvim-notify",
		},
	},
	-- { "github/copilot.vim" },
	{ "sbdchd/neoformat" },
	{
		"stevearc/conform.nvim",
		opts = {},
	},
	{
		"Shougo/deoplete.nvim",
		"roxma/nvim-yarp",
		"roxma/vim-hug-neovim-rpc",
		"ionide/Ionide-vim",
	},
	{ "jlcrochet/vim-razor" },
	-- {
	-- 	"mrcjkb/rustaceanvim",
	-- 	version = "^4", -- Recommended
	-- 	lazy = false, -- This plugin is already lazy
	-- },
	{
		"booperlv/nvim-gomove",
	},
	{
		"iabdelkareem/csharp.nvim",
		dependencies = {
			"williamboman/mason.nvim", -- Required, automatically installs omnisharp
			"mfussenegger/nvim-dap",
			"Tastyep/structlog.nvim", -- Optional, but highly recommended for debugging
		},
		config = function()
			require("mason").setup()
            require"csharp".setup({
	lsp = {
		-- When set to false, csharp.nvim won't launch omnisharp automatically.
		enable = true,
		-- When set, csharp.nvim won't install omnisharp automatically. Instead, the omnisharp instance in the cmd_path will be used.
		cmd_path = nil,
		-- The default timeout when communicating with omnisharp
		default_timeout = 1000,
		-- Settings that'll be passed to the omnisharp server
		enable_editor_config_support = true,
		organize_imports = true,
		load_projects_on_demand = false,
		enable_analyzers_support = true,
		enable_import_completion = false,
		include_prerelease_sdks = true,
		analyze_open_documents_only = false,
		enable_package_auto_restore = true,
		-- Launches omnisharp in debug mode
		debug = false,
		-- The capabilities to pass to the omnisharp server
		capabilities = nil,
		-- on_attach function that'll be called when the LSP is attached to a buffer
		on_attach = nil,
	},
	logging = {
		-- The minimum log level.
		level = "INFO",
	},
	dap = {
		-- When set, csharp.nvim won't launch install and debugger automatically. Instead, it'll use the debug adapter specified.
		--- @type string?
		adapter_name = nil,
	},
}) -- Mason setup must run before csharp
		end,
	},
})
