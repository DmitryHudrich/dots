local builtin = require("telescope.builtin")

vim.g.mapleader = " "


-- NeoTree
vim.keymap.set("n", "<leader>e", ":Neotree float toggle<CR>")
vim.keymap.set("n", "<leader>o", ":Neotree git_status<CR>")

vim.keymap.set("n", "f", ":Format<CR>", { desc = "format document" })

-----------------------------------------------
-- lsp 
vim.keymap.set("n", "<leader>cd", function()
	vim.diagnostic.open_float()
end, { desc = "code diagnostic" })

vim.keymap.set("v", "<leader>ca", function()
	vim.lsp.buf.code_action()
end, { desc = "code action" })

vim.keymap.set({"n","v"}, "<leader>ca", function()
	vim.lsp.buf.code_action()
end, { desc = "code action" })

vim.keymap.set("n", "<leader>ca", function()
	vim.lsp.buf.code_action()
end, { desc = "code action" })

vim.keymap.set("n", "<leader>cr", function()
	vim.lsp.buf.rename()
end, { desc = "rename symbol" })

vim.keymap.set("n", "<leader>cf", function()
	builtin.lsp_references()
end, { desc = "references" })

vim.keymap.set("n", "<leader>cs", function()
	vim.lsp.buf.signature_help()
end, { desc = "signature help" })

vim.keymap.set("n", "gr", function()
	vim.lsp.buf.references()
end, { desc = "LSP Goto Reference" })

vim.keymap.set("n", "gd", function()
	vim.lsp.buf.definition()
end, { desc = "LSP Goto Definition" })

vim.keymap.set("n", "<leader>ch", ":ClangdSwitchSourceHeader<CR>", { desc = "change header" })

-----------------------------------------
-- finding
vim.keymap.set("n", "<leader>fo", function()
	builtin.oldfiles()
end, { desc = "find old files" })

vim.keymap.set("n", "<leader>fg", function()
	builtin.live_grep()
end, { desc = "live grep" })

vim.keymap.set("n", "<leader>ff", function()
	builtin.find_files()
end, { desc = "find files" })



-- formatting
vim.api.nvim_create_user_command("Format", function(args)
  local range = nil
  if args.count ~= -1 then
    local end_line = vim.api.nvim_buf_get_lines(0, args.line2 - 1, args.line2, true)[1]
    range = {
      start = { args.line1, 0 },
      ["end"] = { args.line2, end_line:len() },
    }
  end
  require("conform").format({ async = true, lsp_fallback = true, range = range })
end, { range = true })
