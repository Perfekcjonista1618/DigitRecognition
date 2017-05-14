﻿using FANNCSharp;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
#if FANN_FIXED
using FANNCSharp.Fixed;
using DataType = System.Int32;
#elif FANN_DOUBLE
using FANNCSharp.Double;
using DataType = System.Double;
#else
using FANNCSharp.Float;
using DataType = System.Single;
#endif


namespace GeneratorBitMap
{
    class NeuralNetwork
    {
        public NeuralNet net { get; set; }
        const float learning_rate = 0.7f; //
        const uint num_layers = 3; // ilość warst
        const uint num_input = 200; // ilosc neuronow w wejsciowej warstwie
        const uint num_hidden = 80; // ilosc neuronow w ukrytych
        const uint num_output = 10; // ilosc neuronow w wyjsciowej
        const float desired_error = 0.0001f; //dopuszczalny błąd
        const uint max_iterations = 3000; //max iteracji nauki
        const uint iterations_between_reports = 1000; // nie potrzene
        private readonly string _path = "";
        public NeuralNetwork(string path)
        {
            net = new NeuralNet(NetworkType.LAYER, num_layers, num_input, num_hidden, num_output);
            //typ warstwowa , ilosc warstw , wejscia, ukryta 1 (80) ,ukryta 2(60),wyjscia 
            _path = path;
        }
        public void Train()
        {
            net.LearningRate = learning_rate;
            net.ActivationSteepnessHidden = 0.8F;
            net.ActivationSteepnessOutput = 1.0F;
            net.ActivationFunctionHidden = ActivationFunction.SIGMOID_SYMMETRIC_STEPWISE; //funkcja aktywacji sigmoidalna stepowa w wartwie ukrytej
            net.ActivationFunctionOutput = ActivationFunction.SIGMOID; //funkcja aktywacji sigmoidalna w wartwie wyjsciowej
            net.TrainingAlgorithm = TrainingAlgorithm.TRAIN_RPROP; //rodzaj nauki - propagacja wsteczna

            using (TrainingData data = new TrainingData())
            {
                if (data.ReadTrainFromFile(_path)) // czy plik istnieje jak tak to czyta
                {
                    net.InitWeights(data); //inicjalizowanie wag wartośćiami losowymi
                    net.TrainOnData(data, max_iterations, iterations_between_reports, desired_error); //nauka sieci
                }
            }
        }
    }
}
