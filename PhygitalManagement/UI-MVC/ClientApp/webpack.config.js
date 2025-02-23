const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: {
        //site&index
        index: './src/ts/index/index.ts',
        site: './src/ts/index/site.ts',
        languageDropdown: './src/ts/selectLanguage/languageDropdown.ts',
        //subthemes
        deleteSubthemes: './src/ts/subtheme/deleteSubthemes.ts',
        subthemeActivateOrDeactivate: './src/ts/subtheme/activeDeactiveSubtheme.ts',
        addResponseOptions: './src/ts/subtheme/addResponseOptions.ts',
        editSubthemes: './src/ts/subtheme/editSubtheme.ts',
        addQuestions: "./src/ts/subtheme/addQuestions.ts",
        editQuestions: "./src/ts/subtheme/editQuestions.ts",
        //questions
        questions: './src/ts/questions.ts',
        formSubmitOpinion: './src/ts/questions/formSubmitOpinion.js',
        loadNextOrPreviousSubtheme: './src/ts/questions/loadNextOrPreviousSubtheme.ts',
        //projects
        project: "./src/ts/project/project.ts",
        projectDetails: "./src/ts/project/projectDetails.ts",
        //validation
        validation: './src/ts/validation.ts',
        loginCopyUsers: "./src/ts/loginCopyUsers.js",
        
    },
    output: {
        filename: '[name].entry.js',
        path: path.resolve(__dirname, '..', 'wwwroot', 'dist'),
        clean: true
    },
    devtool: 'source-map',
    mode: 'development',
    resolve: {
        extensions: [".ts", ".js"],
        extensionAlias: {'.js': ['.js', '.ts']}
    },
    module: {
        rules: [
            {
                test: /\.ts$/i,
                use: ['ts-loader'],
                exclude: /node_modules/
            },
            {
                test: /\.s?css$/,
                use: [{ loader: MiniCssExtractPlugin.loader }, 'css-loader', 'sass-loader']
            },
            {
                test: /\.(png|svg|jpg|jpeg|gif|webp)$/i,
                type: 'asset'
            },
            {
                test: /\.(eot|woff(2)?|ttf|otf|svg)$/i,
                type: 'asset'
            }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].css"
        })
    ]
};
