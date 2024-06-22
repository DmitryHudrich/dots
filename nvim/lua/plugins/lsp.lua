local lspconfig = require("lspconfig")
local capabilities = require("cmp_nvim_lsp").default_capabilities()

lspconfig.lua_ls.setup({})

lspconfig.pyright.setup({})

lspconfig.protoling.setup({})
-- configure Ionide / FSharp server
lspconfig.ionide.setup({
	capabilities = capabilities,
	on_attach = function(client, bufnr)
		client.notify("workspace/didChangeConfiguration", { settings = client.config.settings })
	end,
})

lspconfig.clangd.setup({})

-- lspconfig.csharp_ls.setup{}

-- lspconfig.omnisharp.setup({
-- 	capabilities = capabilities,
-- 	cmd = {
-- 		-- "export", "OMNISHARPHOME=/home/hudric;",
-- 		"omnisharp",
-- 	},
-- 	settings = {
-- 		-- FormattingOptions = {
-- 		--   -- Enables support for reading code style, naming convention and analyzer
-- 		--   -- settings from .editorconfig.
-- 		--   EnableEditorConfigSupport = true,
-- 		--   -- Specifies whether 'using' directives should be grouped and sorted during
-- 		--   -- document formatting.
-- 		--   OrganizeImports = nil,
-- 		--
-- 		--   NewLinesForBracesInTypes = false,
-- 		--   NewLinesForBracesInMethods = false,
-- 		--   NewLinesForBracesInProperties = false,
-- 		--   NewLinesForBracesInAccessors = false,
-- 		--   NewLinesForBracesInAnonymousMethods = false,
-- 		--   NewLinesForBracesInControlBlocks = false,
-- 		--   NewLinesForBracesInAnonymousTypes = false,
-- 		--   NewLinesForBracesInObjectCollectionArrayInitializers = false,
-- 		--   NewLinesForBracesInLambdaExpressionBody = false,
-- 		--
-- 		--   NewLineForElse = false,
-- 		--   NewLineForCatch = false,
-- 		--   NewLineForFinally = false,
-- 		--   NewLineForMembersInObjectInit = false,
-- 		--   NewLineForMembersInAnonymousTypes = false,
-- 		--   NewLineForClausesInQuery = false,
-- 		-- },
-- 		MsBuild = {
-- 			-- If true, MSBuild project system will only load projects for files that
-- 			-- were opened in the editor. This setting is useful for big C# codebases
-- 			-- and allows for faster initialization of code navigation features only
-- 			-- for projects that are relevant to code that is being edited. With this
-- 			-- setting enabled OmniSharp may load fewer projects and may thus display
-- 			-- incomplete reference lists for symbols.
-- 			LoadProjectsOnDemand = nil,
-- 		},
-- 		RoslynExtensionsOptions = {
-- 			-- Enables support for roslyn analyzers, code fixes and rulesets.
-- 			EnableAnalyzersSupport = true,
-- 			-- Enables support for showing unimported types and unimported extension
-- 			-- methods in completion lists. When committed, the appropriate using
-- 			-- directive will be added at the top of the current file. This option can
-- 			-- have a negative impact on initial completion responsiveness,
-- 			-- particularly for the first few completion sessions after opening a
-- 			-- solution.
-- 			EnableImportCompletion = true,
-- 			-- Only run analyzers against open files when 'enableRoslynAnalyzers' is
-- 			-- true
-- 			AnalyzeOpenDocumentsOnly = nil,
-- 		},
-- 		Sdk = {
-- 			-- Specifies whether to include preview versions of the .NET SDK when
-- 			-- determining which version to use for project loading.
-- 			IncludePrereleases = true,
-- 		},
-- 	},
-- })
