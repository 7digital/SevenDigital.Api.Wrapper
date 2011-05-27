require 'albacore'
require 'FileUtils'

BUILDTOOLS		= "C:/TeamCityBuildTools"

require File.expand_path("#{File.dirname(__FILE__)}/build/rake/deploy_tasks.rb")
require File.expand_path("#{File.dirname(__FILE__)}/build/build.rb")
require File.expand_path("#{File.dirname(__FILE__)}/build/nunit_test.rb")
require File.expand_path("#{File.dirname(__FILE__)}/build/metrics.rb")
require File.expand_path("#{File.dirname(__FILE__)}/build/lib/source_index.rb")
require File.expand_path("#{File.dirname(__FILE__)}/build/lib/update_dependencies.rb")

SOLUTION		= 'SevenDigital.Api.Wrapper'
ROOT_PROJECT_NAME	= "#{SOLUTION}"

NUNIT_LAUNCHER_PARAMS = "v4.0 x86 NUnit-2.5.3"

NCOVER_FOLDER = "ncover/v3.4.16"
NUNIT_CONSOLE	= "#{BUILDTOOLS}/nunit/2.5.7/bin/net-4.0/NUnit-Console.exe"
NDEPEND_CONFIG = "build/conf/ndepend.config.xml"
	
desc "default"
task :default do
    puts 'no tasks yet'
end

task :build_and_unit_test => [:build_solution, :run_unit_tests, :git_source_index]
task :metrics => [:build_solution,  :create_metrics_dashboard_report, :create_ncover_report, :create_ndepend_report]

sourceindex :git_source_index do |si|
	si.command = "#{BUILDTOOLS}/srcsrv/gitindex.cmd"
	si.solution_folder = "."
end