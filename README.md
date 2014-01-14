This program builds ARFF file from some text records. ARFF file is suitable for text classification in machine learning.

Format of input file: Each line contains pair of class and the text message which are separated by space. 

Example of input file: Imagine we wants to train a classification algorithm to recognize language. So then  the input file may looks like follow

ENG People who refuse to clean up after their dogs should be punished," say that they should be "sent to prisons so lonely that the inmates have to pay spiders for sex..

SVK Dnes je pekný den. Žiadne povodne sa nekonali.

CZK znamy si dal palacinky v ruske restauraci a uz je 2 tydny v nemocnici! on to nezaplatil?


Features:
- unigrams, bigrams
- frequency, pointwise mutual information
- stopwords, Inverse Document Frequency
- Czech stemmer
- morphology in SGLM format