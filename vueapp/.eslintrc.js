module.exports = {
    extends: ["plugin:vue/base"],
    parser: 'vue-eslint-parser',
    parserOptions: {
        parser: 'espree', 
        ecmaVersion: 8, 
        sourceType: 'module'
    },
    plugins: [
        'vue'
    ],
    overrides: [
        {
            files: ['src/**/Abonent.vue', 'src/**/Disrepair.vue',
                'src/**/Executor.vue', 'src/**/Mode.vue',
                'src/**/Price.vue', 'src/**/Remain.vue',
                'src/**/Request.vue', 'src/**/Service.vue',
                'src/**/Street.vue', 'src/**/Unit.vue',],
            rules: {
                'vue/multi-word-component-names': 'off'
            }
        }
    ]
}